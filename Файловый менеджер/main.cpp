/****************************************************************************
**
** Copyright (C) 2020 The Qt Company Ltd.
** Contact: https://www.qt.io/licensing/
**
** This file is part of the examples of the Qt Toolkit.
**
** $QT_BEGIN_LICENSE:BSD$
** Commercial License Usage
** Licensees holding valid commercial Qt licenses may use this file in
** accordance with the commercial license agreement provided with the
** Software or, alternatively, in accordance with the terms contained in
** a written agreement between you and The Qt Company. For licensing terms
** and conditions see https://www.qt.io/terms-conditions. For further
** information use the contact form at https://www.qt.io/contact-us.
**
** BSD License Usage
** Alternatively, you may use this file under the terms of the BSD license
** as follows:
**
** "Redistribution and use in source and binary forms, with or without
** modification, are permitted provided that the following conditions are
** met:
**   * Redistributions of source code must retain the above copyright
**     notice, this list of conditions and the following disclaimer.
**   * Redistributions in binary form must reproduce the above copyright
**     notice, this list of conditions and the following disclaimer in
**     the documentation and/or other materials provided with the
**     distribution.
**   * Neither the name of The Qt Company Ltd nor the names of its
**     contributors may be used to endorse or promote products derived
**     from this software without specific prior written permission.
**
**
** THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
** "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
** LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
** A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
** OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
** SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
** LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
** DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
** THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
** (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
** OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE."
**
** $QT_END_LICENSE$
**
****************************************************************************/

#include <QApplication>
#include <QStyleOptionViewItem>
#include <QFileSystemModel>
#include <QFileIconProvider>
#include <QScreen>
#include <QScroller>
#include <QTreeView>
#include <QLineEdit>
#include <QDir>
#include <QMessageBox>
#include <QFuture>
#include <QPainter>
#include <QVBoxLayout>
#include <QFuture>
#include <QFutureWatcher>
#include <QSortFilterProxyModel>
#include <QProgressDialog>
#include <QPushButton>
#include <QModelIndex>
#include <QStyleOptionButton>
#include <QFileInfoList>
#include <QAbstractItemModel>
#include <QMouseEvent>
#include <QStyledItemDelegate>
#include <QCommandLineParser>
#include <QCommandLineOption>
#include "buttondelegate.h"
int main(int argc, char *argv[])
{
    QApplication app(argc, argv);

    QCoreApplication::setApplicationVersion(QT_VERSION_STR);
    QCommandLineParser parser;
    parser.setApplicationDescription("Qt Dir View Example");
    parser.addHelpOption();
    parser.addVersionOption();
    QCommandLineOption dontUseCustomDirectoryIconsOption("c", "Set QFileSystemModel::DontUseCustomDirectoryIcons");
    parser.addOption(dontUseCustomDirectoryIconsOption);
    QCommandLineOption dontWatchOption("w", "Set QFileSystemModel::DontWatch");
    parser.addOption(dontWatchOption);
    parser.addPositionalArgument("directory", "The directory to start in.");
    parser.process(app);
    const QString rootPath = QDir::homePath();
    QFileSystemModel model;
    model.setRootPath(rootPath);
    if (parser.isSet(dontUseCustomDirectoryIconsOption))
        model.setOption(QFileSystemModel::DontUseCustomDirectoryIcons);
    if (parser.isSet(dontWatchOption))
        model.setOption(QFileSystemModel::DontWatchForChanges);
    model.setFilter(QDir::AllEntries|QDir::NoDotAndDotDot| QDir::Hidden);
    QTreeView tree;
    ButtonDelegate *button = new ButtonDelegate(&tree);
    tree.setItemDelegateForColumn(1,button);
    QLineEdit filterLineEdit;
    QObject::connect(&filterLineEdit,&QLineEdit::textChanged,[&model](const QString &text)
    {
        QStringList nameFilters;
        if (!text.isEmpty())
        {
            nameFilters<<text+"*";
        }
        model.setNameFilters(nameFilters);
        model.setNameFilterDisables(false);
    });
    QWidget* window = new QWidget;
    model.sort(0);
    tree.setModel(&model);
    tree.setRootIndex(model.index(rootPath));
    tree.setItemDelegateForColumn(1, button);
    tree.setAnimated(false);
    tree.setIndentation(20);
    tree.setSortingEnabled(true);
    const QSize availableSize = tree.screen()->availableGeometry().size();
    tree.resize(availableSize / 2);
    tree.setColumnWidth(0, tree.width() / 3);
    // Make it flickable on touchscreens
    QScroller::grabGesture(&tree, QScroller::TouchGesture);
    QVBoxLayout* layout=new QVBoxLayout;
    layout->addWidget(&filterLineEdit);
    layout->addWidget(&tree);
    window->setLayout(layout);
    window->setWindowTitle(QObject::tr("Dir View"));
    window->show();
    tree.setWindowTitle(QObject::tr("Dir View"));
    tree.show();
    return app.exec();
}
