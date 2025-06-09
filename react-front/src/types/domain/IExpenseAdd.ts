import {IDomainId} from "@/types/IDomainId";
import {IDestination} from "@/types/domain/IDestination";
import {ICurrency} from "@/types/domain/ICurrency";
import {IDestinationInTrip} from "@/types/domain/IDestinationInTrip";
import {IExpenseSubCategory} from "@/types/domain/IExpenseSubCategory";

export interface IExpenseAdd extends IDomainId {
    amountOriginal: number
    expenseSubCategoryId: string
    description: string
    tripId: string
    currencyId: string
}