import { Address } from './address';
export interface User{
    id: string;
    username: string;
    email: string;
    name: string;
    surname: string;
    address: Address;
    roles: string[];
}