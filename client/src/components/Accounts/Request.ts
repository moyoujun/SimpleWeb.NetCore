import { AxiosRequestConfig } from 'axios';
import { convertPlainTextToSHA1 } from "@/utils/hash"

export class AccountRequest implements AxiosRequestConfig {
    headers: IAppHeader
    data: any;

    constructor(email: string, password: string, username?: string) {
        this.data = {
            email: email,
            name: username,
            password: password,
        }
        let unixTime = Math.floor(Date.now() / 1000);
        let secret = "apLcGB9UA9RKOeQBhQx5lDNOxX6HGjjcMwnywHLsOablkB2xmsHl8Eioz0cpXbFT"
        let signature = convertPlainTextToSHA1(unixTime + "." + secret + "." + email + "." + password);

        this.headers = {
            timestamp: unixTime.toString(),
            signature: signature,
        };
    }
}

export interface IAppHeader {
    timestamp: string;
    signature: string;
    Authorization?: string;
}