export interface HubInterface{
  id: string;
  name: string;
  city: string;
  address: string;
  longitude: number;
  latitude: number;
  connectedHubs: {
    id: string;
    distance: number;
  }[];
}
