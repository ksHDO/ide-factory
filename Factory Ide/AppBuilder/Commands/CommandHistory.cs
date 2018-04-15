using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBuilder.Commands
{
    public class CommandHistory
    {
        private readonly Stack<IFactoryIdeCommand> m_undoHistory;
        private readonly Stack<IFactoryIdeCommand> m_redoHistory;

        public CommandHistory(int initialSize = 100)
        {
            m_undoHistory = new Stack<IFactoryIdeCommand>(initialSize);
            m_redoHistory = new Stack<IFactoryIdeCommand>(initialSize);
        }

        public void PerformCommand(IFactoryIdeCommand command)
        {
            command.Do();
            m_redoHistory.Clear();
            m_undoHistory.Push(command);
        }

        public void UndoCommand()
        {
            PopCommand(m_undoHistory, m_redoHistory, c => c.Undo());
        }

        public void RedoCommand()
        {
            PopCommand(m_redoHistory, m_undoHistory, c => c.Do());
        }

        private void PopCommand(Stack<IFactoryIdeCommand> pop, Stack<IFactoryIdeCommand> push, Action<IFactoryIdeCommand> action)
        {
            if (pop.Count <= 0) return;
            var command = pop.Pop();
            action(command);
            push.Push(command);
        }
    }
}
