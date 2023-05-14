/**
 * FlightBookingAPI
 * API aplikacije za kupovinu avionskih karata
 *
 * The version of the OpenAPI document: 1.0
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */


/**
 * Zahtev za kupovinu odredjenog broja karata za odabrani let od strane korisnika.
 */
export interface FlightBuyTicketsRequest { 
    /**
     * Broj karata za kupovinu.
     */
    amount: number;
    userId: string;
}

