import { EntityService } from "./EntityService";
import {ITrip} from "@/types/domain/ITrip";
import { ITripAdd } from "@/types/domain/ITripAdd";
import {IDestination} from "@/types/domain/IDestination";
import {ICurrency} from "@/types/domain/ICurrency";

export class CurrencyService extends EntityService<ICurrency, ICurrency> {
    constructor(){
        super('currency')
    }
}

