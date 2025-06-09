import { EntityService } from "./EntityService";
import {ITrip} from "@/types/domain/ITrip";
import { ITripAdd } from "@/types/domain/ITripAdd";
import {IExpense} from "@/types/domain/IExpense";
import {IExpenseAdd} from "@/types/domain/IExpenseAdd";
import {IExpenseCategory} from "@/types/domain/IExpenseCategory";

export class ExpenseCategoryService extends EntityService<IExpenseCategory, IExpenseCategory> {
    constructor(){
        super('expenseCategory')
    }
}

