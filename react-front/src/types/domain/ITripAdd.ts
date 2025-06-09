import {IDomainId} from "@/types/IDomainId";
import {IDestination} from "@/types/domain/IDestination";
import {ICurrency} from "@/types/domain/ICurrency";
import {IDestinationInTrip} from "@/types/domain/IDestinationInTrip";
import {IExpense} from "@/types/domain/IExpense";

export interface ITripAdd extends IDomainId {
    budgetOriginal: number
    startDate: Date
    endDate: Date
    name: string
    public: boolean
    currencyId: string | null;
    destinationId: string | null;
}