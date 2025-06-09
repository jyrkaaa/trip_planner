import { EntityService } from "./EntityService";
import {ITrip} from "@/types/domain/ITrip";
import { ITripAdd } from "@/types/domain/ITripAdd";
import {IExpense} from "@/types/domain/IExpense";
import {IExpenseAdd} from "@/types/domain/IExpenseAdd";

export class ExpenseService extends EntityService<IExpense, IExpenseAdd> {
    constructor(){
        super('expense')
    }
}

