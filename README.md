![image](https://github.com/spacementhelper/spacementhelper/assets/144666955/c2382035-b297-483b-bfe4-9593b6dc6fdb)
![image](https://github.com/spacementhelper/spacementhelper/assets/144666955/ef676a2a-5d73-4b90-9a8c-b5c851111302)


zip files：                                                                                 


[SpacemeshHelper V0.0.6](https://raw.githubusercontent.com/spacementhelper/spacementhelper/dba5b8814b40a3bc183194955cf691a1fc198e05/SpacemeshHelper__V0.0.6.zip)




如果你有能力,请下载源代码自行编译.


使用方法:

先从官网 https://spacemesh.io/start/  下载Spacemesh钱包，安装完成后，创建钱包，并同步好节点。默认节点数据在C:\Users\Administrator\AppData\Roaming\Spacemesh\node-data，你可以修改到其它目录）
P盘：
运行本程序，填写节点IP，如果是本机，就保持默认127.0.0.1:9092，选择你要P盘的显卡，文件保存路径，P盘单个文件大小，一共要P多大的盘，
例如你有一个4TB的硬盘Z盘，你需要P 4T的文件，你选择单文件4G，就会P 1024个文件，单文件16G就会P 256个文件，如果因为时间关系，你只想P 1TB
的文件，你就选对应的大小，可以创建多个目录，分别P对应的大小，本例中你可以创建4个目录，P 4个1TB，这样可以防止出错导致整个4TB都不能使用，
选择好后，点击部署，如果你是多张不同型号的显卡，你可以修改性能高的显卡P的文件多一些，也可以保持默认分配。最后点击开始创建文件。

如果主节点没有运行，会报错。不能获取ID。

挂盘：

当你P完盘后，你需要挂盘等待注册，点击挂盘选项，在数据目录点击右键，添加P好盘的目录，（尽量给目录命名顺序添加，方便你后续方便查看，比如上面例子中创建四个目录可以P001,P002,P003,P004）
添加好后点击开始挂盘，会在程序目录里创建对应的节点目录。填写你的钱包地址。
注意：如果你不想多个节点同时同步节点占用大量网络带宽，你可以在这步操作后关闭程序。确保你的主钱包节点同步为最新。复制节点数据目录里的所有文件（state.sql p2p 等等...）到程序目录对应的节点目录（程序目录下\GO_POSTDATA\1\Spacemesh）多个节点都复制最新的到每个节点目录。复制完成后再运行本程序，就会很快同步完成。等待注册。

如果你的机器配置不高，你可能不能运行太多节点。





tg:https://t.me/+x-MRWuvAnUMyYWVl
