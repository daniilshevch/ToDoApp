import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import TaskList from './features/tasks/TaskList';
import './App.css'

function App() {
    return (
        <div className="App">
            <h1>Щоденник Задач</h1>
            <TaskList />
        </div>
    )
      
}

export default App;
