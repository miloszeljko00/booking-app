export * from './flight.service';
import { FlightService } from './flight.service';
export * from './user.service';
import { UserService } from './user.service';
export const APIS = [FlightService, UserService];
