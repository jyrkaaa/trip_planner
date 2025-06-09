import {IDomainId} from "@/types/IDomainId";

export interface IExercise extends IDomainId {
    name: string;
    desc: string | null;
    date: string;
    targetId: string | null;
    guideId: string | null;
    categoryId: string | null;
}