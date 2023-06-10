export interface CreateHostNotification{
    receiveAnswerForCreatedRequest: boolean,
    receiveAnswerForCanceledReservation: boolean,
    receiveAnswerForHostRating: boolean,
    receiveAnswerForAccommodationRating: boolean,
    receiveAnswerForHighlightedHostStatus: boolean
    hostEmail: string;
}