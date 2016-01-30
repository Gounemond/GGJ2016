// ------------------------------------------------------------------------------
//  _______   _____ ___ ___   _   ___ ___ 
// |_   _\ \ / / _ \ __/ __| /_\ | __| __|
//   | |  \ V /|  _/ _|\__ \/ _ \| _|| _| 
//   |_|   |_| |_| |___|___/_/ \_\_| |___|
// 
// This file has been generated automatically by TypeSafe.
// Any changes to this file may be lost when it is regenerated.
// https://www.stompyrobot.uk/tools/typesafe
// 
// TypeSafe Version: 1.2.1-Unity5
// 
// ------------------------------------------------------------------------------



public sealed class SRScenes {
    
    private SRScenes() {
    }
    
    private const string _tsInternal = "1.2.1-Unity5";
    
    public static global::TypeSafe.Scene mainmenu {
        get {
            return __all[0];
        }
    }
    
    public static global::TypeSafe.Scene SpiderPhotoSession {
        get {
            return __all[1];
        }
    }
    
    private static global::System.Collections.Generic.IList<global::TypeSafe.Scene> __all = new global::System.Collections.ObjectModel.ReadOnlyCollection<global::TypeSafe.Scene>(new global::TypeSafe.Scene[] {
                new global::TypeSafe.Scene("mainmenu", 0),
                new global::TypeSafe.Scene("SpiderPhotoSession", 1)});
    
    public static global::System.Collections.Generic.IList<global::TypeSafe.Scene> All {
        get {
            return __all;
        }
    }
}
