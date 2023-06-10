export interface HostNotification{
    lastModified: string;
    receiveAnswerForCreatedRequest: boolean,
    receiveAnswerForCanceledReservation: boolean,
    receiveAnswerForHostRating: boolean,
    receiveAnswerForAccommodationRating: boolean,
    receiveAnswerForHighlightedHostStatus: boolean
    hostEmail: string;
}