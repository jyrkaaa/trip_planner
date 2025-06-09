import {IDomainId} from "@/types/IDomainId";
import {IDestination} from "@/types/domain/IDestination";
import {ICurrency} from "@/types/domain/ICurrency";
import {IDestinationInTrip} from "@/types/domain/IDestinationInTrip";
import {IExpense} from "@/types/domain/IExpense";

export interface ITrip extends IDomainId {
    budgetOriginal: number
    budgetBase: number
    name: string
    startDate: Date
    endDate: Date
    public: boolean
    currencyId: string | null;
    currency: ICurrency;
    expenses: IExpense[];
    destinationsInTrip: IDestinationInTrip[] | null;

}