"use client"
import { SubmitHandler, useForm} from "react-hook-form";
import { useContext, useState } from "react";
import { AccountService } from "@/services/AccountService";
import { AccountContext } from "@/context/AccountContext";
import { useRouter } from "next/navigation";


export default function Login() {
    const accountService = new AccountService();
    const { setAccountInfo } = useContext(AccountContext);
    const router = useRouter();
    const [errorMessage, setErrorMessage] = useState("");

    type Inputs = {
        email: string,
        password: string,
    }
    const {
        register,
        handleSubmit,
        formState: { errors }
    } = useForm<Inputs>({
        defaultValues: {
            email: 'jurgen@gmail.com',
            password: 'StrongPa1!ssword'
        }
    });

    const onSubmit : SubmitHandler<Inputs> = async (data: Inputs) => {
        console.log(data);
        setErrorMessage("Loading");
        try {
            const result = await accountService.loginAsync(data.email, data.password);
            console.log(result);
            if (result.errors) {
                setErrorMessage(result.statusCode + " - " + result.errors[0]);
                return;
            }
            setErrorMessage("");

            setAccountInfo!({
                jwt: result.data!.jwt,
                refreshToken: result.data!.refreshToken,

            });
            localStorage.setItem("_jwt", result.data!.jwt);
            localStorage.setItem("_refreshToken", result.data!.refreshToken);
            router.push("/");
        } catch (err) {
            setErrorMessage("Login Failed - " + (err as Error).message);
        }
    };
    return (
        <>
            <div className="row">
                <div className="col-4"></div>
                <div className="col-4">

                    {errorMessage}

                    <form onSubmit={handleSubmit(onSubmit)}>
                        <h2>Login</h2>
                        <hr />

                        <div className="text-danger validation-summary-valid" role="alert" data-valmsg-summary="true">
                        </div>
                        <div className="form-floating mb-3">
                            <input
                                className="form-control"
                                aria-required="true"
                                placeholder="name@example.com"
                                autoComplete="username"
                                type="email"
                                id="Input_Email"
                                {...register("email", { required: true })}
                            />
                            <label className="form-label" htmlFor="Input_Email">Email</label>
                            {
                                errors.email &&
                                <span className="text-danger field-validation-valid" data-valmsg-for="Input.Email" data-valmsg-replace="true">Required!</span>
                            }
                        </div>
                        <div className="form-floating mb-3">
                            <input
                                className="form-control"
                                aria-required="true"
                                placeholder="password"
                                type="password"
                                id="Input_Password"
                                autoComplete="current-password"
                                {...register("password", { required: true })}
                            />
                            <label className="form-label" htmlFor="Input_Password">Password</label>
                            {
                                errors.password &&
                                <span className="text-danger field-validation-valid" data-valmsg-for="Input.Email" data-valmsg-replace="true">Required!</span>
                            }
                        </div>

                        <div>
                            <button id="login-submit" type="submit" className="w-100 btn btn-lg btn-primary">Log in</button>
                        </div>

                    </form>
                </div>
            </div>

        </>
    );
}
