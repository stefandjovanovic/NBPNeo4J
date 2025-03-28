export interface ServiceInterface {
  id: string;
  name: string;
  city: string;
  address: string;
  longitude: number;
  latitude: number;
  connectedToHubId: string;
  hubDistance: number;
}
