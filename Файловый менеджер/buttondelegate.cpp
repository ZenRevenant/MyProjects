#include "buttondelegate.h"
#include <QStyledItemDelegate>
#include <QPushButton>
#include <QApplication>
#include <QFileInfo>
#include <QDebug>
#include <QDir>
#include <QFuture>
#include <QtConcurrent>
#include <QFileSystemModel>
ButtonDelegate::ButtonDelegate(QObject* parent):QStyledItemDelegate(parent){}
    QWidget *ButtonDelegate::createEditor(QWidget *parent, const QStyleOptionViewItem &option, const QModelIndex &index)const
    {
        Q_UNUSED(option)
        Q_UNUSED(index)
        QString filePath = index.data(QFileSystemModel::FilePathRole).toString();
        QFileInfo fileInfo(filePath);
    }
    void ButtonDelegate::paint(QPainter *painter, const QStyleOptionViewItem &option, const QModelIndex &index) const
    {
        QString filePath = index.data(QFileSystemModel::FilePathRole).toString();
        QFileInfo fileInfo(filePath);
        if(fileInfo.isDir())
        {   QStyleOptionButton buttonOption;
            buttonOption.rect=option.rect;
            if (!KnownFiles.contains(filePath))
            {
                buttonOption.text=QString("Обновить");
                buttonOption.state=QStyle::State_Enabled;
            }
            else
            {
                const QStringList units={"Б","Кб","Мб","Гб","Тб"};
                double size=static_cast<double>(KnownFiles.value(filePath));
                short Index=0;
                while(size>=1024 && Index<units.size()-1)
                {
                    size/=1024;
                    Index++;
                }
                buttonOption.text=QString::number(size,'f',2)+" "+units[Index];
                buttonOption.state=QStyle::State_Enabled;
            }
            QApplication::style()->drawControl(QStyle::CE_PushButton,&buttonOption,painter);
        }
    }
    bool  ButtonDelegate::editorEvent(QEvent *event, QAbstractItemModel *model, const QStyleOptionViewItem &option, const QModelIndex &index)
    {
        if (event->type() == QEvent::MouseButtonRelease)
        {
            emit buttonClicked(index);
            QString filePath = index.data(QFileSystemModel::FilePathRole).toString();
            QFuture<quint64> future=QtConcurrent::run(Count,filePath);
            future.waitForFinished();
            quint64 Size=future.result();
            KnownFiles.insert(filePath,Size);
        }
        return 0;
    }
quint64 Count(const QString& filePath)
{
    qint64 size = 0;
    QDir dir(filePath);
        //calculate total size of current directories' files
        for(const auto& filePath : dir.entryList(QDir::Files|QDir::System|QDir::Hidden)) {
            size += QFileInfo(dir, filePath).size();
        }
        //add size of child directories recursively
        for(const auto& childDirPath : dir.entryList(QDir::Dirs|QDir::NoDotAndDotDot|QDir::System|QDir::Hidden)) {
            size += Count(filePath + QDir::separator() + childDirPath);
        }
        return size;

}
