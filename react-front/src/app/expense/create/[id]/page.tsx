"use client";

import {useContext, useEffect, useState} from "react";
import { useRouter, useParams } from "next/navigation";
import { TripService } from "@/services/TripService";
import { ExpenseService } from "@/services/ExpenseService";
import { ExpenseCategoryService } from "@/services/ExpenseCategoryService";
import { IExpenseAdd } from "@/types/domain/IExpenseAdd";
import { IExpenseCategory } from "@/types/domain/IExpenseCategory";
import {AccountContext} from "@/context/AccountContext";
import {CurrencyService} from "@/services/CurrencyService";
import {ICurrency} from "@/types/domain/ICurrency";

export default function AddExpenseForm() {
    const { accountInfo } = useContext(AccountContext);
    const router = useRouter();
    const { id: tripId } = useParams<{ id: string }>();
    const expenseService = new ExpenseService();
    const categoryService = new ExpenseCategoryService();
    const currencyService = new CurrencyService();

    const [form, setForm] = useState<IExpenseAdd>({
        id: "",
        amountOriginal: 0,
        description: "",
        expenseSubCategoryId: "",
        tripId: tripId,
        currencyId: ""
    });

    const [categories, setCategories] = useState<IExpenseCategory[]>([]);
    const [loading, setLoading] = useState(false);
    const [currencies, setCurrency] = useState<ICurrency[]>([]);

    useEffect(() => {
        const fetchCategories = async () => {
            try {
                const result = await categoryService.getAllAsync();
                if (result?.data) {
                    setCategories(result.data);
                }
            } catch (err) {
                console.error("Failed to load categories:", err);
            }
        };const fetchCurrencies = async () => {
            try {
                const result = await currencyService.getAllAsync();
                if (result?.data) {
                    setCurrency(result.data);
                }
            } catch (err) {
                console.error("Failed to load currencys:", err);
            }
        };

        fetchCategories();
        fetchCurrencies();
    }, [accountInfo, router]);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const { name, value } = e.target;
        setForm((prev) => ({
            ...prev,
            [name]: name === "amountOriginal" ? parseFloat(value) : value,
        }));
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);

        try {
            const result = await expenseService.addAsync(form);
            if (result.errors) {
                console.error(result.errors);
                alert("Failed to save expense.");
                return;
            }
            alert("Expense added!");
            router.push(`/trip/edit/${tripId}`);
        } catch (err) {
            console.error("Submission failed:", err);
            alert("Error while submitting.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="container py-5">
            <h2 className="mb-4">Add Expense</h2>

            <form onSubmit={handleSubmit} className="row g-3">
                <div className="col-md-6">
                    <label className="form-label">Amount (Original)</label>
                    <input
                        type="number"
                        className="form-control"
                        name="amountOriginal"
                        min={0}
                        step="0.01"
                        value={form.amountOriginal}
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
                    <label className="form-label">Subcategory</label>
                    <select
                        name="expenseSubCategoryId"
                        className="form-select"
                        value={form.expenseSubCategoryId}
                        onChange={handleChange}
                        required
                    >
                        <option value="">-- Select Subcategory --</option>
                        {categories.map((category) => (
                            category.expenseSubCategories?.length ? (
                                <optgroup key={category.id} label={category.categoryName}>
                                    {category.expenseSubCategories.map((sub) => (
                                        <option key={sub.id} value={sub.id}>
                                            {sub.name}
                                        </option>
                                    ))}
                                </optgroup>
                            ) : null
                        ))}
                    </select>
                </div>

                <div className="col-12">
                    <label className="form-label">Description</label>
                    <input
                        type="text"
                        className="form-control"
                        name="description"
                        value={form.description}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div className="col-12">
                    <button type="submit" className="btn btn-success" disabled={loading}>
                        {loading ? "Saving..." : "Add Expense"}
                    </button>
                </div>
            </form>
        </div>
    );
}
