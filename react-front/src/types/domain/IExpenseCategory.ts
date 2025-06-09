import {IDomainId} from "@/types/IDomainId";
import {IDestination} from "@/types/domain/IDestination";
import {ICurrency} from "@/types/domain/ICurrency";
import {IDestinationInTrip} from "@/types/domain/IDestinationInTrip";
import {IExpense} from "@/types/domain/IExpense";
import {IExpenseSubCategory} from "@/types/domain/IExpenseSubCategory";

export interface IExpenseCategory extends IDomainId {
    categoryName: string
    expenseSubCategories: IExpenseSubCategory[] | null
}