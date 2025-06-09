"use client";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { useContext } from "react";
import {AccountContext} from "@/context/AccountContext";
import {SubmitHandler} from "react-hook-form";
import {AccountService} from "@/services/AccountService";

export default function Header() {
    const { accountInfo, setAccountInfo } = useContext(AccountContext);
    const router = useRouter();
    const accountService = new AccountService();
    type Inputs = {
        jwt: string;
        refreshToken: string;
    }

    const onClick  = async (jwt: string | undefined, refreshToken: string | undefined) => {
        try {
            const result = await accountService.logoutAsync(jwt!, refreshToken!);
            console.log(result);
            if (result.errors) {
                console.log(result.errors);
                return;
            }
        } catch (err) {
            console.error(err);
        }
    };

    return (
        <>
            <nav className="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
                <div className="container">
                    <Link className="navbar-brand" href="/">GymApp</Link>
                    <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target=".navbar-collapse"
                            aria-controls="navbarSupportedContent"
                            aria-expanded="false" aria-label="Toggle navigation">
                        <span className="navbar-toggler-icon"></span>
                    </button>
                    <div className="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                        <ul className="navbar-nav flex-grow-1">
                            <li className="nav-item">
                                <Link className="nav-link text-dark" href="/">Home</Link>
                            </li>

                            {accountInfo?.jwt &&
                                <li className="nav-item dropdown">
                                    <a className="nav-link text-dark dropdown-toggle" href="javascript:{}" id="userDropdown"
                                       role="button" data-bs-toggle="dropdown" aria-expanded="false">User</a>
                                    <div className="dropdown-menu" aria-labelledby="userDropdown">
                                        <Link className="dropdown-item text-dark" href="/trip">Trips</Link>
                                    </div>
                                </li>
                            }

                        </ul>
                        <ul className="navbar-nav">
                            <li className="nav-item">
                        {accountInfo?.jwt &&
                                    <a className="nav-link text-dark" href="#" onClick={() => {
                                        setAccountInfo!({});
                                        onClick(accountInfo?.jwt, accountInfo?.refreshToken).then(r =>
                                        router.push("/login"));
                                    }}>Logout</a>
                        }
                        {!accountInfo?.jwt &&
                        <Link className="dropdown-item text-dark" href="/login">Login</Link>
                        }
                            </li>
                        </ul>
                    </div>
                </div>
            </nav>


        </>
    );
}
