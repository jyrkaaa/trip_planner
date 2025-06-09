import {IResultObject} from "@/types/IResultObject";
import {ILoginDto} from "@/types/ILoginDTO";
import {BaseService} from "@/services/BaseService";
import { AxiosError} from "axios";


export class AccountService extends BaseService {

    async loginAsync(email: string, password: string): Promise<IResultObject<ILoginDto>> {
        const url = "Account/Login"
        try {
            const loginData = {
                email: email,
                password: password
            }
                const response = await this.axiosInstance.post<ILoginDto>(url, loginData);
                if (response.status < 300) {
                    return {
                        data: response.data
                    }
                }
                return {
                    errors: [response.status.toString() + " " + response.statusText]
                }
        } catch (error: any) {
            return {
                errors: [JSON.stringify(error)]
            };
        }
    }
    async logoutAsync(jwt : string, refreshToken : string): Promise<IResultObject<ILoginDto>> {
        const logoutData = {
            refreshToken: refreshToken,
        }
        const options = {
            headers: {
                Authorization: `Bearer ${jwt}`,
            },
        }
        const url = "Account/Logout"
        try {
            const response = await this.axiosInstance.post<ILoginDto>(url, logoutData, options);
            if (response.status < 300) {
                return {
                    data: response.data
                }
            }
            return {
                errors: [response.status.toString() + " " + response.statusText]
            }
        } catch (error: any) {
            return {
                errors: [JSON.stringify(error)]
            };
        }
    }
    async registerAsync(email: string, pwd: string): Promise<IResultObject<ILoginDto>> {
        const registerData = {
            email: email,
            password: pwd
        }
        const url = "Account/Register";
        try {
            const response = await this.axiosInstance.post<ILoginDto>(url, registerData);
            if (response.status < 300) {
                return {
                    data: response.data
                }
            }
            return {
                errors: [response.status.toString() + " " + response.statusText]
            }
        } catch (error: any) {
            return {
                errors: [JSON.stringify(error)]
            };
        }
    }

}
