import {IDomainId} from "@/types/IDomainId";
import {IExercise} from "@/types/domain/IExercise";

export interface IExerciseCategoryAdd extends IDomainId{
    name: string;
    exercises: IExercise[] | null;
}