"use client";

import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import { TripService } from "@/services/TripService";
import { ITripAdd } from "@/types/domain/ITripAdd";
import { ICurrency } from "@/types/domain/ICurrency";
import { IDestination } from "@/types/domain/IDestination";
import {DestinationService} from "@/services/DestinationService";
import {CurrencyService} from "@/services/CurrencyService";

export default function TripCreateForm() {
    const tripService = new TripService();
    const destinationService = new DestinationService();
    const currencyService = new CurrencyService();
    const router = useRouter();

    const [form, setForm] = useState<ITripAdd>({
        id: "", // optional: will be ignored server-side
        name: "",
        budgetOriginal: 0,
        startDate: new Date(),
        endDate: new Date(),
        public: false,
        currencyId: null,
        destinationId: null,
    });

    const [currencies, setCurrencies] = useState<ICurrency[]>([]);
    const [destinations, setDestinations] = useState<IDestination[]>([]);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        // Load currency and destination options
        const loadOptions = async () => {
            try {
                const [currencyRes, destinationRes] = await Promise.all([
                    currencyService.getAllAsync(),
                    destinationService.getAllAsync()
                ]);

                if (currencyRes?.data) setCurrencies(currencyRes.data);
                if (destinationRes?.data) setDestinations(destinationRes.data);
            } catch (e) {
                console.error("Error loading options:", e);
            }
        };
        loadOptions();
    }, []);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const { name, value, type, checked } = e.target;
        const newValue = type === "checkbox" ? checked : value;
        setForm((prev) => ({
            ...prev,
            [name]: name.includes("Date") ? new Date(value) : newValue
        }));
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        try {
            const result = await tripService.addAsync(form);
            if (result.errors) {
                console.error(result.errors);
                alert("Failed to create trip.");
            } else {
                alert("Trip created successfully!");
                router.push("/trip");
            }
        } catch (err) {
            console.error("Submission error:", err);
            alert("Error submitting form.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="container py-5">
            <h2 className="mb-4">Create New Trip</h2>

            <form onSubmit={handleSubmit} className="row g-3">
                <div className="col-md-6">
                    <label className="form-label">Trip Name</label>
                    <input
                        type="text"
                        className="form-control"
                        name="name"
                        value={form.name}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div className="col-md-6">
                    <label className="form-label">Budget (Original)</label>
                    <input
                        type="number"
                        className="form-control"
                        name="budgetOriginal"
                        value={form.budgetOriginal}
                        onChange={handleChange}
                        required
                        min={0}
                        step="0.01"
                    />
                </div>

                <div className="col-md-6">
                    <label className="form-label">Start Date</label>
                    <input
                        type="date"
                        className="form-control"
                        name="startDate"
                        value={form.startDate.toISOString().split("T")[0]}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div className="col-md-6">
                    <label className="form-label">End Date</label>
                    <input
                        type="date"
                        className="form-control"
                        name="endDate"
                        value={form.endDate.toISOString().split("T")[0]}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div className="col-md-6">
                    <label className="form-label">Currency</label>
                    <select
                        className="form-select"
                        name="currencyId"
                        value={form.currencyId ?? ""}
                        onChange={handleChange}
                        required
                    >
                        <option value="">-- Select Currency --</option>
                        {currencies.map((c) => (
                            <option key={c.id} value={c.id}>
                                {c.code} - {c.name}
                            </option>
                        ))}
                    </select>
                </div>

                <div className="col-md-6">
                    <label className="form-label">Destination</label>
                    <select
                        className="form-select"
                        name="destinationId"
                        value={form.destinationId ?? ""}
                        onChange={handleChange}
                        required
                    >
                        <option value="">-- Select Destination --</option>
                        {destinations.map((d) => (
                            <option key={d.id} value={d.id}>
                                {d.countryName}
                            </option>
                        ))}
                    </select>
                </div>

                <div className="col-12">
                    <div className="form-check">
                        <input
                            className="form-check-input"
                            type="checkbox"
                            name="public"
                            checked={form.public}
                            onChange={handleChange}
                            id="publicCheckbox"
                        />
                        <label className="form-check-label" htmlFor="publicCheckbox">
                            Make trip public
                        </label>
                    </div>
                </div>

                <div className="col-12">
                    <button type="submit" className="btn btn-success" disabled={loading}>
                        {loading ? "Saving..." : "Create Trip"}
                    </button>
                </div>
            </form>
        </div>
    );
}
