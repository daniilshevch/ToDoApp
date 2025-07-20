import React, { useState, useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { fetchTasks, patchTask, deleteTask, type Task } from './tasksSlice';
import type { RootState, AppDispatch } from '../../app/store';
import './TaskList.css';
import { Modal, Form, Input, Select, DatePicker } from 'antd';
import TaskCard from './TaskCard';
import CreateTaskButton from './CreateTaskButton';

import dayjs from 'dayjs';


const TaskList: React.FC = () =>
{
    const dispatch = useDispatch<AppDispatch>();
    const [editModalVisible, setEditModalVisible] = useState(false);
    const [form] = Form.useForm();
    const [selectedTask, setSelectedTask] = useState<Task | null>(null);
    const handleEdit = (task: Task) => {
        setSelectedTask(task);
        form.setFieldsValue({
            title: task.title,
            description: task.description,
            status: task.status,
            deadline: dayjs(task.deadline)
        });
        setEditModalVisible(true);
    };
    const handleUpdate = () => {
        form.validateFields().then(values => {
            const updated = {
                ...selectedTask!,
                ...values,
                deadline: values.deadline.toDate()
            };
            dispatch(patchTask(updated));
            setEditModalVisible(false);
            form.resetFields();
        })
    };
    const handleDelete = (taskId: number) =>
    {
        dispatch(deleteTask(taskId));
    }
    const { tasks, loading, error } = useSelector((state: RootState) => state.tasks);
    useEffect(() => {
        dispatch(fetchTasks());
    }, [dispatch]);
    const renderTasksByStatus = (status: 'ToDo' | 'InProgress' | 'Done') =>
    {
        return tasks.filter(task => task.status === status)
            .map(task => <TaskCard key={task.id} task={task} onEdit={handleEdit} onDelete={handleDelete} />)
    };
    if (loading) return <p>Завантаження задач</p>;
    if (error) return <p>Помилка: {error}</p>
    return (
        <div className="task-page">
        <div className = "task-page-header">
            <h2>Список задач</h2>
            <CreateTaskButton />
        </div>
            <div className="task-columns">
                <div className="column">
                    <h2 className="todo-header">Треба розпочати виконання</h2>
                    {renderTasksByStatus('ToDo')}
                </div>
                <div className="column">
                    <h2 className = "inprogress-header">В процесі виконання</h2>
                    {renderTasksByStatus('InProgress')}
                </div>
                <div className="column">
                    <h2 className = "done-header">Зроблено</h2>
                    {renderTasksByStatus('Done')}
                </div>
            </div>
            <Modal
                title="Редагування задачі"
                open={editModalVisible}
                onOk={handleUpdate}
                onCancel={() => setEditModalVisible(false)}
                okText="Оновити"
                cancelText="Скасувати"
            >
                <Form form={form} layout="vertical">
                    <Form.Item name="title" label="Назва" rules={[{ required: true }]}>
                        <Input />
                    </Form.Item>
                    <Form.Item name="description" label="Зміст" rules={[{ required: true }]}>
                        <Input.TextArea rows={3} />
                    </Form.Item>
                    <Form.Item name="status" label="Статус" rules={[{ required: true }]}>
                        <Select
                            options={[
                                { value: 'ToDo', label: 'ToDo' },
                                { value: 'InProgress', label: 'InProgress' },
                                { value: 'Done', label: 'Done' }
                            ]}
                        />
                    </Form.Item>
                    <Form.Item name="deadline" label="Дедлайн" rules={[{ required: true }]}>
                        <DatePicker format="DD.MM.YYYY" style={{ width: '100%' }} />
                    </Form.Item>
                </Form>
            </Modal>
        </div>
    );
}
export default TaskList;