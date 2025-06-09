"use client";

import { useContext, useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";
import { TripService } from "@/services/TripService";
import { ITrip } from "@/types/domain/ITrip";
import { AccountContext } from "@/context/AccountContext";

export default function Home() {
  const tripService = new TripService();
  const { accountInfo } = useContext(AccountContext);
  const router = useRouter();
  const [data, setData] = useState<ITrip[] | null>(null);
  const [searchQuery, setSearchQuery] = useState("");

  useEffect(() => {
    if (!accountInfo?.jwt) {
      router.push("/login");
      return;
    }

    const fetchData = async () => {
      try {
        const result = await tripService.getAllAsync(true);
        if (result.errors) {
          console.error(result.errors);
          return;
        }
        setData(result.data!);
      } catch (error) {
        console.error("Error fetching trips:", error);
      }
    };

    fetchData();
  }, [accountInfo, router]);

  const handleDelete = async (tripId: string) => {
    if (!confirm("Are you sure you want to delete this trip?")) return;

    try {
      const result = await tripService.deleteAsync(tripId);
      if (result.errors) {
        console.error(result.errors);
        alert("Failed to delete trip.");
        return;
      }
      setData(data!.filter((t) => t.id !== tripId));
    } catch (err) {
      console.error("Error deleting trip:", err);
      alert("Error deleting trip.");
    }
  };

  const filteredTrips = data?.filter((trip) => {
    const q = searchQuery.toLowerCase();
    const name = trip.name?.toLowerCase() || "";
    const date = new Date(trip.startDate).toLocaleDateString().toLowerCase();
    return name.includes(q) || date.includes(q);
  });

  if (data == null) {
    return <div className="container mt-5">Loading...</div>;
  }

  return (
      <div className="container py-5">
        <div className="d-flex justify-content-between align-items-center mb-4">
          <h1 className="mb-0">Trips Dashboard</h1>
          <Link href="/trip/create" className="btn btn-primary">
            + Add New Trip
          </Link>
        </div>

        <div className="mb-4">
          <input
              type="text"
              className="form-control"
              placeholder="Search by name or date..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
          />
        </div>

        <div className="row g-4">
          {filteredTrips?.map((trip) => {
            const totalExpenses = trip.expenses?.reduce((sum, e) => sum + (e.baseAmount ?? 0), 0) ?? 0;
            const remaining = trip.budgetBase - totalExpenses;

            return (
                <div key={trip.id} className="col-md-6 col-lg-4">
                  <div className="card h-100 shadow-sm">
                    <div className="card-body d-flex flex-column">
                      <h5 className="card-title">{trip.name || "Unnamed Trip"}</h5>
                      <h6 className="card-subtitle mb-2 text-muted">
                        {new Date(trip.startDate).toLocaleDateString()} -{" "}
                        {new Date(trip.endDate).toLocaleDateString()}
                      </h6>

                      <ul className="list-group list-group-flush my-3">
                        <li className="list-group-item">
                          <strong>Currency:</strong> {trip.currency?.code} ({trip.currency?.name})
                        </li>
                        <li className="list-group-item">
                          <strong>Budget (Base):</strong> {trip.budgetBase.toFixed(2)}
                        </li>
                        <li className="list-group-item">
                          <strong>Expenses (Base):</strong> {totalExpenses.toFixed(2)}
                        </li>
                        <li className="list-group-item">
                          <strong>Remaining:</strong>{" "}
                          <span className={remaining < 0 ? "text-danger" : "text-success"}>
                                                {remaining.toFixed(2)}
                                            </span>
                        </li>
                        <div className="px-2 mt-3">
                          <label className="form-label mb-1"><strong>Progress</strong></label>
                          <div className="progress" style={{height: "20px"}}>
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
                      </ul>

                      <div className="mt-auto d-flex flex-column gap-2">
                        <Link href={`/trip/edit/${trip.id}`} className="btn btn-outline-primary btn-sm">
                          View Details
                        </Link>
                        <Link
                            href={`/expense/create/${trip.id}`}
                            className="btn btn-outline-success btn-sm"
                        >
                          Add Expense
                        </Link>
                        <button
                            onClick={() => handleDelete(trip.id)}
                            className="btn btn-outline-danger btn-sm"
                        >
                          Delete Trip
                        </button>
                      </div>
                    </div>
                  </div>
                </div>
            );
          })}
        </div>
      </div>
  );
}
