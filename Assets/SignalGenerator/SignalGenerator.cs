using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class SignalGenerator : MonoBehaviour {

    SawWave sawAudioWave;
    SquareWave squareAudioWave;
    SinusWave sinusAudioWave;

    SinusWave amplitudeModulationOscillator;
    SinusWave frequencyModulationOscillator;

    [Header("Channels / Output")]
    [Range(-1.0f, 1.0f)]
    public float stereoPan = 0;

    public SignalData signal;

    private double sampleRate;  // samples per second
    private double dataLen;     // the data length of each channel
    double chunkTime;
    double dspTimeStep;
    double currentDspTime;

    public void loadPreset(SignalData data) {
        if (data == null) return;
        signal = new SignalData(data);
    }

    public SignalData exportPreset() {
        return new SignalData(signal);
    }

    void Awake() {
        sawAudioWave = new SawWave();
        squareAudioWave = new SquareWave();
        sinusAudioWave = new SinusWave();

        amplitudeModulationOscillator = new SinusWave();
        frequencyModulationOscillator = new SinusWave();

        sampleRate = AudioSettings.outputSampleRate;
    }

    void OnAudioFilterRead(float[] data, int channels) {
        /* This is called by the system
		suppose: sampleRate = 48000
		suppose: data.Length = 2048
		suppose: channels = 2
		then:
		dataLen = 2048/2 = 1024
		chunkTime = 1024 / 48000 = 0.0213333... so the chunk time is around 21.3 milliseconds.
		dspTimeStep = 0.0213333 / 1024 = 2.083333.. * 10^(-5) = 0.00002083333..sec = 0.02083 milliseconds
			keep note that 1 / dspTimeStep = 48000 ok!		
		*/

        currentDspTime = AudioSettings.dspTime;
        dataLen = data.Length / channels;   // the actual data length for each channel
        chunkTime = dataLen / sampleRate;   // the time that each chunk of data lasts
        dspTimeStep = chunkTime / dataLen;  // the time of each dsp step. (the time that each individual audio sample (actually a float value) lasts)

        double preciseDspTime;
        for (int i = 0; i < dataLen; i++) { // go through data chunk
            preciseDspTime = currentDspTime + i * dspTimeStep;
            double signalValue = 0.0;

            double currentSinusFrequency = signal.sineFrequency;
            double currentSquareFrequency = signal.squareFrequency;
            double currentSawFrequency = signal.sawFrequency;

            if (signal.freqModAmplitude > 0) {
                double freqOffset = (signal.freqModAmplitude * 100f * currentSinusFrequency * 0.75) / 100.0;
                double offset = mapValueD(frequencyModulationOscillator.calculateSignalValue(preciseDspTime, signal.freqModAmplitude), -1.0, 1.0, -freqOffset, freqOffset);
                currentSinusFrequency += offset;

                freqOffset = (signal.freqModAmplitude * 100f * currentSquareFrequency * 0.75) / 100.0;
                offset = mapValueD(frequencyModulationOscillator.calculateSignalValue(preciseDspTime, signal.freqModFrequency), -1.0, 1.0, -freqOffset, freqOffset);
                currentSquareFrequency += offset;

                freqOffset = (signal.freqModAmplitude * 100f * currentSawFrequency * 0.75) / 100.0;
                offset = mapValueD(frequencyModulationOscillator.calculateSignalValue(preciseDspTime, signal.ampModFrequency), -1.0, 1.0, -freqOffset, freqOffset);
                currentSawFrequency += offset;
            }

            if (signal.sineFrequency > 0f) {
                signalValue += signal.sineAmplitude * sinusAudioWave.calculateSignalValue(preciseDspTime, currentSinusFrequency);
            }
            if (signal.sawFrequency > 0f) {
                signalValue += signal.sawAmplitude * sawAudioWave.calculateSignalValue(preciseDspTime, currentSawFrequency);
            }
            if (signal.squareFrequency > 0) {
                signalValue += signal.squareAmplitude * squareAudioWave.calculateSignalValue(preciseDspTime, currentSquareFrequency);
            }

            if (signal.ampModFrequency > 0) {
                signalValue *= mapValueD(amplitudeModulationOscillator.calculateSignalValue(preciseDspTime, signal.ampModFrequency), -1.0, 1.0, 0.0, 1.0);
            }

            float x = signal.globalAmplitude * 0.5f * (float)signalValue; // What if no 0.5???

            //Channel 1
            if (stereoPan > 0) data[i * channels] = x * (1 - stereoPan);
            else data[i * channels] = x;
            //Channel 2
            if (stereoPan < 0) data[i * channels + 1] = x * (stereoPan + 1);
            else data[i * channels + 1] = x;
        }

    }

    float mapValue(float referenceValue, float fromMin, float fromMax, float toMin, float toMax) {
        /* This function maps (converts) a Float value from one range to another */
        return toMin + (referenceValue - fromMin) * (toMax - toMin) / (fromMax - fromMin);
    }

    double mapValueD(double referenceValue, double fromMin, double fromMax, double toMin, double toMax) {
        /* This function maps (converts) a Double value from one range to another */
        return toMin + (referenceValue - fromMin) * (toMax - toMin) / (fromMax - fromMin);
    }

}


