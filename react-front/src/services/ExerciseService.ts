import { EntityService } from "./EntityService";
import {IExerciseCategory} from "@/types/domain/IExerciseCategory";
import {IExerciseCategoryAdd} from "@/types/domain/IExerciseCategoryAdd";

export class ExerciseCategoryService extends EntityService<IExerciseCategory, IExerciseCategoryAdd> {
    constructor(){
        super('Exercise')
    }
}

