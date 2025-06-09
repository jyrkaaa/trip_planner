import {IDomainId} from "@/types/IDomainId";
import {IExercise} from "@/types/domain/IExercise";

export interface IExerciseCategory extends IDomainId{
    name: string;
    exercises: IExercise[] | null;
}