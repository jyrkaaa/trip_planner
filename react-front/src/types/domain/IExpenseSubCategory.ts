import {IDomainId} from "@/types/IDomainId";
import {IDestination} from "@/types/domain/IDestination";
import {ICurrency} from "@/types/domain/ICurrency";
import {IDestinationInTrip} from "@/types/domain/IDestinationInTrip";
import {IExpenseCategory} from "@/types/domain/IExpenseCategory";

export interface IExpenseSubCategory extends IDomainId {
    name: string
    expenseCategoryId: string
    expenseCategory: IExpenseCategory


}