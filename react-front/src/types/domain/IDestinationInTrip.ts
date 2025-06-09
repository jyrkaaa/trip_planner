import {IDomainId} from "@/types/IDomainId";
import {IDestination} from "@/types/domain/IDestination";

export interface IDestinationInTrip extends IDomainId {
    tripId: string
    destinationId: string
    destination: IDestination[]
}