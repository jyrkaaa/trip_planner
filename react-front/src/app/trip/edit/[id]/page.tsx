"use client";

import { useEffect, useState, useContext } from "react";
import { useParams, useRouter } from "next/navigation";
import { TripService } from "@/services/TripService";
import { ITrip } from "@/types/domain/ITrip";
import { AccountContext } from "@/context/AccountContext";
import Link from "next/link";

export default function TripDetails() {
    const { id } = useParams<{ id: string }>();
    const router = useRouter();
    const tripService = new TripService();
    const { accountInfo } = useContext(AccountContext);

    const [trip, setTrip] = useState<ITrip | null>(null);

    useEffect(() => {
        if (!accountInfo?.jwt) {
            router.push("/login");
            return;
        }

        const fetchTrip = async () => {
            try {
                const result = await tripService.getAsync(id);
                if (result.errors) {
                    console.error(result.errors);
                    alert("Failed to fetch trip.");
                    return;
                }
                setTrip(result.data!);
            } catch (err) {
                console.error("Error fetching trip:", err);
            }
        };

        if (id) fetchTrip();
    }, [id, accountInfo, router]);

    if (!trip) {
        return <div className="container mt-5">Loading trip details...</div>;
    }

    const totalExpenses = trip.expenses?.reduce((sum, e) => sum + (e.baseAmount ?? 0), 0) ?? 0;
    const remaining = trip.budgetBase - totalExpenses;

    return (
        <div className="container py-5">
            <h2 className="mb-3">{trip.name}</h2>
            <p className="text-muted">
                {new Date(trip.startDate).toLocaleDateString()} -{" "}
                {new Date(trip.endDate).toLocaleDateString()}
            </p>

            <ul className="list-group mb-4">
                <div className="mb-4">
                    <label className="form-label mb-1"><strong>Budget Usage</strong></label>
                    <div className="progress" style={{height: "24px"}}>
                        <div
                            className="progress-bar bg-success"
                            role="progressbar"
                            style={{width: `${Math.max((remaining / trip.budgetBase) * 100, 0)}%`}}
                        >
                            {remaining > 0 ? `${((remaining / trip.budgetBase) * 100).toFixed(1)}% Left` : ""}
                        </div>
                        <div
                            className="progress-bar bg-danger"
                            role="progressbar"
                            style={{width: `${Math.min((totalExpenses / trip.budgetBase) * 100, 100)}%`}}
                        >
                            {totalExpenses > 0 ? `${((totalExpenses / trip.budgetBase) * 100).toFixed(1)}% Spent` : ""}
                        </div>
                    </div>
                </div>
                <li className="list-group-item">
                    <strong>Currency:</strong> {trip.currency?.code} ({trip.currency?.name})
                </li>
                <li className="list-group-item">
                    <strong>Budget (Base):</strong> {trip.budgetBase.toFixed(2)}
                </li>
                <li className="list-group-item">
                    <strong>Total Expenses:</strong> {totalExpenses.toFixed(2)}
                </li>
                <li className="list-group-item">
                    <strong>Remaining:</strong>{" "}
                    <span className={remaining < 0 ? "text-danger" : "text-success"}>
                        {remaining.toFixed(2)}
                    </span>
                </li>
                <li className="list-group-item">
                    <strong>Public:</strong> {trip.public ? "Yes" : "No"}
                </li>
            </ul>

            <div className="d-flex justify-content-between align-items-center mb-3">
                <h4 className="mb-0">Expenses</h4>
                <Link href={`/expense/create/${trip.id}`} className="btn btn-sm btn-success">
                    + Add Expense
                </Link>
            </div>

            {trip.expenses && trip.expenses.length > 0 ? (
                <table className="table table-bordered">
                    <thead className="table-light">
                    <tr>
                        <th>Description</th>
                        <th>Original Amount</th>
                        <th>Base Amount</th>
                        <th>Category</th>
                    </tr>
                    </thead>
                    <tbody>
                    {trip.expenses.map((e) => (
                        <tr key={e.id}>
                            <td>{e.description || "(no description)"}</td>
                            <td>{e.originalAmount.toFixed(2)}</td>
                            <td>{e.baseAmount.toFixed(2)}</td>
                            <td>{e.expenseSubCategory?.name || "Uncategorized"}</td>
                        </tr>
                    ))}
                    </tbody>
                </table>
            ) : (
                <p className="text-muted">No expenses recorded for this trip.</p>
            )}
        </div>
    );
}
