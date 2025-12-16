const DEFAULT_BASE = import.meta.env.VITE_API_BASE ?? "http://localhost:5249/api";
const BASE_URL = DEFAULT_BASE.replace(/\/+$/, "");

export async function getWeeklySummary(): Promise<WeeklySummary> {
    const response = await fetch(`${BASE_URL}/summary/week`);
    return response.json();
}

export async function getWeeklyShifts(): Promise<Shift[]> {
    const response = await fetch(`${BASE_URL}/shifts/week`);
    return response.json();
}

export async function getEmployees(): Promise<Employee[]> {
    const response = await fetch(`${BASE_URL}/employees`);
    return response.json();
}

export async function createShift(request: CreateShiftRequest): Promise<void> {
    const payload = {
        employeeId: request.employeeId,
        date: request.date,
        startTime: `${request.date}T${request.startTime}:00`,
        endTime: `${request.date}T${request.endTime}:00`,
    };

    await fetch(`${BASE_URL}/shifts`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload),
    });
}