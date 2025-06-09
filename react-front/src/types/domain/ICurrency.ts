import {IDomainId} from "@/types/IDomainId";

export interface ICurrency extends IDomainId {
    code: string
    name: string
    rate: number
}