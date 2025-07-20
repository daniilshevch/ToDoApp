import React from 'react';
import { Button } from 'antd';
import { EditOutlined, DeleteOutlined } from '@ant-design/icons';
import type { Task } from './tasksSlice';
import './TaskCard.css';

interface TaskCardProps {
    task: Task;
    onEdit: (task: Task) => void;
    onDelete: (taskId: number) => void;
}

const TaskCard: React.FC<TaskCardProps> = ({ task, onEdit, onDelete }) =>
{
    return (
        <div key={task.id} className="task-card">
            <h4 className="task-title">{task.title}</h4>
            <h5 className="task-description-header">Задача:</h5>
            <p className="task-description">{task.description}</p>
            <p className="task-deadline">
                ⏰ Дедлайн: {new Date(task.deadline).toLocaleDateString()}
            </p>
            <Button
                icon={<EditOutlined />}
                type="default"
                size="small"
                onClick={() => { onEdit(task) }}
            >
                Редагувати
            </Button>
            <Button
                icon={<DeleteOutlined />}
                danger
                size="small"
                onClick={() => onDelete(task.id)}
            >
                Видалити
            </Button>
        </div>
    );
}
export default TaskCard;