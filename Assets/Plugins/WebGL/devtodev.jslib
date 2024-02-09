var devtodevPlugin = {
    /**
     * Get IndexedDB availability
     * @return bool
     */
    IsIndexedDBAvailable: function() {
        var isAvailable = false;
        window.indexedDB = window.indexedDB ||
            window.mozIndexedDB ||
            window.webkitIndexedDB ||
            window.msIndexedDB;

        if (window.indexedDB) isAvailable = true;

        return isAvailable;
    },

    /**
     * Get storage availability
     * @return bool
     */
    IsStorageAvailable: function() {
        try {
            const key = "__example_key__";
            window.localStorage.setItem(key, key);
            window.localStorage.removeItem(key);
            return true;
        } catch (e) {
            return false;
        }
    },

    /**
     * Initialize hooks for WebGL.
     */
    InitializeWebGL: function() {
        window.onfocus = function() {
            try {
                gameInstance.SendMessage('[devtodev_AsyncOperationDispatcher]', 'OnTabFocusEvent');
            } catch (e) {}
            try {
                unityInstance.SendMessage('[devtodev_AsyncOperationDispatcher]', 'OnTabFocusEvent');
            } catch (e) {}
        }
    },

    /**
     * @param key string
     * @return void
     */
    RemoveItem: function(key) {
        try {
            window.localStorage.removeItem(Pointer_stringify(key));
        } catch (e) {}

    },

    /**
     * @param key string
     * @param value string
     * @return void
     */
    SetItem: function(key, value) {
        try {
            window.localStorage.setItem(Pointer_stringify(key), Pointer_stringify(value));
        } catch (e) {}
    },

    /**
     * @param key string
     * @return object || null
     */
    GetItem: function(key) {
        try {
            var result = window.localStorage.getItem(Pointer_stringify(key));
            result = result === 'undefined' ? null : result;
            if (result != null) {
                var buffer = _malloc(lengthBytesUTF8(result) + 1);
                writeStringToMemory(result, buffer);
                return buffer;
            }
        } catch (e) {}

        return null;
    },

    /**
     * @param key string
     * @return bool
     */
    IsExistItem: function(key) {
        try {
            var result = window.localStorage.getItem(key);
            result = result === 'undefined' ? false : true;
            return result;
        } catch (e) {}

        return false;
    },

    /**
     * Get UserAgent string.
     */
    GetUserAgent: function() {
        var userAgentString = navigator.userAgent;
        var buffer = _malloc(lengthBytesUTF8(userAgentString) + 1);
        writeStringToMemory(userAgentString, buffer);
        return buffer;
    }
};
mergeInto(LibraryManager.library, devtodevPlugin);