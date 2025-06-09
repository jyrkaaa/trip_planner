import { EntityService } from "./EntityService";
import {ITrip} from "@/types/domain/ITrip";
import { ITripAdd } from "@/types/domain/ITripAdd";
import {IDestination} from "@/types/domain/IDestination";

export class DestinationService extends EntityService<IDestination, IDestination> {
    constructor(){
        super('destination')
    }
}

