import { EntityService } from "./EntityService";
import {ITrip} from "@/types/domain/ITrip";
import { ITripAdd } from "@/types/domain/ITripAdd";
import {IResultObject} from "@/types/IResultObject";
import {AxiosError} from "axios";

export class TripService extends EntityService<ITrip, ITripAdd> {
    constructor(){
        super('trip')
    }
    override async getAllAsync(includePublic: boolean = false): Promise<IResultObject<ITrip[]>> {
        try {
            const url = includePublic ? `${this.basePath}?includePublic=true` : this.basePath;

            const response = await this.axiosInstance.get<ITrip[]>(url);

            console.log("getAll response", response);

            if (response.status <= 300) {
                return {
                    statusCode: response.status,
                    data: response.data,
                };
            }

            return {
                statusCode: response.status,
                errors: [(response.status.toString() + " " + response.statusText).trim()],
            };
        } catch (error) {
            console.log("error: ", (error as AxiosError).message);
            return {
                statusCode: (error as AxiosError).status ?? 0,
                errors: [(error as AxiosError).code ?? "???"],
            };
        }
    }
}

