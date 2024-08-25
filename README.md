# 「VR刺身タンポポ」ソースコード

<img src="https://user-images.githubusercontent.com/882466/120896471-b9b6fc00-c65c-11eb-8bf9-46e6f5d82b51.jpg" width="640">

Unityを使用して制作したMeta Questアプリ「VR刺身タンポポ」 https://vrsashimi.com/ のソースコードです。アセット等は含まれていないためビルドはできません。[Unity1週間ゲームジャム](https://unityroom.com/unity1weeks)に参加されている方々が[ソースコードを公開](https://blog.naichilab.com/entry/2020/08/30/112346)されているのを真似して公開してみました。

ライブラリはVContainerとUniTaskを主に使用。[FrameSynthesisフォルダ](https://github.com/korinVR/VRSashimiTanpopoSource/tree/main/Assets/FrameSynthesis)が主にVRヘッドセット関連、[VRSashimiTanpopoフォルダ](https://github.com/korinVR/VRSashimiTanpopoSource/tree/main/Assets/VRSashimiTanpopo)がゲーム本体です。元リポジトリのほうでは、GitHub Actionsを使用してMeta Quest版、Oculus Rift版、Steam VR版の自動ビルドを走らせています。

アプリとしてはシンプルなので、規模も5000行ほどでそんなに難しいことはしていないのですが（どっちかというとコード以外のところでいろいろやってたり。別途記事を書くかもしれません）、こんな感じのプログラムで刺身タンポポが動いてるんだなーと面白がっていただけたら幸いです。下記の記事・スライドもあわせてご参照ください。

- [Unity \+ Meta Quest開発メモ \- フレームシンセシス](https://tech.framesynthesis.co.jp/unity/metaquest/)
- [「VR開発におけるマルチプラットフォーム対応」](https://docs.google.com/presentation/d/e/2PACX-1vRamEOhM7Zk-mkZRYiUAFZsiOlAt8vRN98x00-e-CZ4C45cVI4MAC_lqmpVfytaAfXi4FyBcX8_4SvO/pub?slide=id.g74f9420c67_0_1314)（Gotanda.unity #16）
- [GitHub ActionsのセルフホストランナーでUnityビルドする \- フレームシンセシス](https://tech.framesynthesis.co.jp/github/actions-unity/)

プログラム：こりん ([@korinVR](https://x.com/korinVR))
