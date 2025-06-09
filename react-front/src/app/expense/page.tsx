"use client"

import {useContext, useEffect, useState} from "react";
import {AccountContext} from "@/context/AccountContext";
import {useRouter} from "next/navigation";
import {IExercise} from "@/types/domain/IExercise";
import {set} from "react-hook-form";
import Link from "next/link";
import {IExerciseCategory} from "@/types/domain/IExerciseCategory";
import {ExerciseCategoryService} from "@/services/ExerciseService";
export default function Expenses() {
    const exerciseService = new ExerciseCategoryService();
    const { accountInfo } = useContext(AccountContext);
    const router =useRouter();
    const [data, setData] = useState<IExerciseCategory[]>([]);
    const [searchQuery, setSearchQuery] = useState("");


    useEffect(() => {
        if (!accountInfo?.jwt) {
            router.push("/login");
        }
        const fetchData = async () => {
            try {
                const result = await exerciseService.getAllAsync();
                if (result.errors) {
                    console.log(result.errors);
                    return;
                }
                setData(result.data!);
            } catch (error) {
                console.error("Error fetching data:", error);
            }
        };
        fetchData();
    }, []);
    if (data.length === 0) {
        return "Loading..."
    }
    // Filter based on search query
    const filteredData = Object.entries(data).filter(([category, exercises]) =>
        exercises.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
        exercises.exercises?.some(ex =>
            ex.name.toLowerCase().includes(searchQuery.toLowerCase())
        )
    );

    return (
        <>
            <div className="container mt-5">
                <h1>All Exercises</h1>

                <Link href="/makeExercise">
                    <button type="button" className="btn btn-success mb-4">Add New Exercise</button>
                </Link>
                <div className="mb-4">
                    <input
                        type="text"
                        className="form-control"
                        placeholder="Search by category or exercise name"
                        value={searchQuery}
                        onChange={(e) => setSearchQuery(e.target.value)}
                    />
                </div>
                {data.length === 0 ? (
                    <div>No Exercises found.</div>
                ) : (
                    // Flatten and filter all exercises across all categories
                    data
                        .flatMap((category) => category.exercises?.map((ex) => ({
                            ...ex,
                            categoryName: category.name // Optional: retain category name for display
                        })))
                        .filter((exercise) =>
                            exercise?.name.toLowerCase().includes(searchQuery.toLowerCase())
                        )
                        .map((exercise) => (
                            <div key={exercise!.id} className="col-md-4 mb-4 exercise-card">
                                <div className="card">
                                    <div className="card-body">
                                        <h5 className="card-title">{exercise!.name}</h5>
                                        <p className="card-text">
                                            {exercise!.desc || "No description provided"}
                                        </p>
                                        <p className="text-muted"><small>{exercise!.categoryName}</small></p>
                                        <button className="btn btn-danger">Delete Exercise</button>
                                    </div>
                                </div>
                            </div>
                        ))
                )}
            </div>
        </>
    )};