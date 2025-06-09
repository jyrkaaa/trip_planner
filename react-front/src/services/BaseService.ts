import axios, {AxiosInstance} from "axios";
import {useContext} from "react";
import {AccountContext} from "@/context/AccountContext";
import {ILoginDto} from "@/types/ILoginDTO";

export abstract class BaseService {
    protected axiosInstance: AxiosInstance;
    private setAccountInfo = useContext(AccountContext).setAccountInfo;

    constructor() {
        this.axiosInstance
            = axios.create({
            baseURL: "http://localhost:5049/api/v1/",
            headers: {
                "Content-Type": "application/json",
                Accept: "application/json",
            },
        });


        this.axiosInstance.interceptors.request.use(
            (config) => {
                const token = localStorage.getItem("_jwt");
                if (token) {
                    config.headers.Authorization = `Bearer ${token}`;
                }
                return config;
            },
            (error) => {
                return Promise.reject(error);
            }
        );

        this.axiosInstance.interceptors.response.use(
            (response) => {
                return response;
            },

            async (error) => {
                const oringinalRequest = error.config;
                if (error.response && error.response.status === 401 && !oringinalRequest._retry) {
                    oringinalRequest._retry = true;
                    try {
                        const jwt = localStorage.getItem("_jwt");
                        const refreshToken = localStorage.getItem("_refreshToken");
                        const response = await axios.post<ILoginDto>(
                            "http://localhost:5049/api/v1/account/RenewRefreshToken",
                            {
                                jwt: jwt,
                                refreshToken: refreshToken,
                            }
                        );

                        console.log("renewRefreshToken", response);

                        if (response && response.status <= 300) {
                            localStorage.setItem("_jwt", response.data.jwt);
                            localStorage.setItem("_refreshToken", response.data.refreshToken);
                            oringinalRequest.headers.Authorization = `Bearer ${response.data.jwt}`;

                            this.setAccountInfo!({
                                jwt: response.data.jwt,
                                refreshToken: response.data.refreshToken,
                            });

                            return this.axiosInstance(oringinalRequest);
                        }

                        return Promise.reject(error);
                    } catch (error) {
                        console.error("Error refreshing token:", error);
                        return Promise.reject(error);
                    }

                }
                return Promise.reject(error);
            }
        );
    }

}