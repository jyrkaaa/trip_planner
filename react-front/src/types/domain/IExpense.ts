import {IDomainId} from "@/types/IDomainId";
import {IDestination} from "@/types/domain/IDestination";
import {ICurrency} from "@/types/domain/ICurrency";
import {IDestinationInTrip} from "@/types/domain/IDestinationInTrip";
import {IExpenseSubCategory} from "@/types/domain/IExpenseSubCategory";

export interface IExpense extends IDomainId {
    originalAmount: number
    baseAmount: number
    expenseSubCategoryId: string
    description: string
    expenseSubCategory: IExpenseSubCategory


}