#ifndef BUTTONDELEGATE_H
#define BUTTONDELEGATE_H

#include <QObject>
#include <QStyledItemDelegate>
#include <QWidget>
#include <QPushButton>

class ButtonDelegate:public QStyledItemDelegate
{
    Q_OBJECT
public:
    ButtonDelegate(QObject *parent=nullptr);
    void paint(QPainter *painter, const QStyleOptionViewItem &option, const QModelIndex &index) const override;
        QWidget *createEditor(QWidget *parent, const QStyleOptionViewItem &option, const QModelIndex &index) const override;
        bool editorEvent(QEvent *event, QAbstractItemModel *model, const QStyleOptionViewItem &option, const QModelIndex &index) override;
        QMap<QString,quint64>KnownFiles;
signals:
    void buttonClicked(const QModelIndex) const;
};
quint64 Count(const QString& filePath);
#endif // BUTTONDELEGATE_H
