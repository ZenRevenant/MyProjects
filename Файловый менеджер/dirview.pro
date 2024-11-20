QT += widgets
QT += concurrent
requires(qtConfig(treeview))

SOURCES       = main.cpp \
    buttondelegate.cpp

# install
target.path = $$[QT_INSTALL_EXAMPLES]/widgets/itemviews/dirview
INSTALLS += target

HEADERS += \
    buttondelegate.h
