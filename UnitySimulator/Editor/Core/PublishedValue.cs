using System.Diagnostics;

namespace Hulan.PilloSDK.Simulator.Core {
  /// <summary>
  /// A field with a change callback.
  /// </summary>
  /// <typeparam name="FieldType">The field's type</typeparam>
  internal class PublishedValue<FieldType> {
    /// <summary>
    /// The field's protected value field. This field is used to store the
    /// field's value privately.
    /// </summary>
    FieldType value;

    /// <summary>
    /// The field's change callback. This callback is invoked when the
    /// field's value changes to a new value. The new value is passed as
    /// an argument to the callback.
    /// </summary>
    readonly System.Action<FieldType> callback;

    /// <summary>
    /// Creates a new field with a change callback.
    /// </summary>
    /// <param name="callback">The callback method.</param>
    internal PublishedValue(System.Action<FieldType> callback) {
      this.callback = callback;
      value = default;
      callback(value);
    }

    /// <summary>
    /// Creates a new field with a change callback.
    /// </summary>
    /// <param name="value">The initial value.</param>
    /// <param name="callback">The callback method.</param>
    internal PublishedValue(FieldType value, System.Action<FieldType> callback) {
      this.callback = callback;
      this.value = value;
      callback(value);
    }

    /// <summary>
    /// Gets or sets the field's value. When the value changes, the callback
    /// is invoked.
    /// </summary>
    internal FieldType Value {
      get => value;
      set {
        if (this.value.Equals(value)) {
          return;
        }
        this.value = value;
        callback(value);
      }
    }
  }
}