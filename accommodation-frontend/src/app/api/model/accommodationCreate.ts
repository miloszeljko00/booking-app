export interface AccommodationCreate{
    name: string;
    capacity: {min: number, max: number}
    address: {city: string, street:string, number:string, country:string};
    benefits: number[];
    reserveAutomatically: boolean;
    pictures: {filename: string}[];
    priceCalculation: number;
    pricePerGuest: number[]
}