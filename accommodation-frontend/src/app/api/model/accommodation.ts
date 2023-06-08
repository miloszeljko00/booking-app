export interface Accommodation{
    name: string;
    min: number;
    max: number;
    price: number;
    priceCalculation: string;
    address: string;
    benefits: string;
    id: string;
    pictures: {fileName: string, base64: string}[]
    hostEmail: string;
}