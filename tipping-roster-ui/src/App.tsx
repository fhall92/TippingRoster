import "./index.css";
import WeeklySummary from "./components/WeeklySummary";
import RosterTable from "./components/RosterTable";
import ShiftForm from "./components/ShiftForm";

export default function App() {
    return (
        <div style={{ padding: "20px" }}>
            <h1>Tipping & Roster App</h1>
            <WeeklySummary />
            <RosterTable />
            <ShiftForm />
        </div>
    );
}
