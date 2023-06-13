export interface GetSuggestedFlightsDto{
  placeOfDeparture: string,
  firstDayDate: string | Date,
  placeOfArrival: string,
  lastDayDate: string | Date
}
