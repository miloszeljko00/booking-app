export interface ReservationByGuest{
    id :string;
    start: string;
    end: string;
    totalPrice: number;
    isCanceled: boolean;
    guestNumber: number;
    accommodationName: string;
    accommodationId: string;
}