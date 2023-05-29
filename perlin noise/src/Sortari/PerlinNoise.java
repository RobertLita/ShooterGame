package Sortari;

import java.awt.*;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import java.util.Random;
import javax.imageio.ImageIO;

public class PerlinNoise {
    private float x;
    private float y;

    private static final Random random = new Random();
    private static final int octaves = 6;
    private static final float persistence = 0.5f;

    static PerlinNoise randomGr(int x, int y, int seed)
    {
        float rd =(float) (2920.f * Math.sin((x+seed)*21942.f+(y+seed)*171324.f+8912.f)*Math.cos((x+seed)*23157.f*(y+seed)*217832.f+9758.f));
        PerlinNoise pr = new PerlinNoise();
        pr.x=(float) Math.sin(rd);
        pr.y=(float) Math.sin(rd);
        return pr;
    }

    static float randomGrD(int x, int y, float a, float b, int seed)
    {
        PerlinNoise gradient = randomGr(x, y, seed);
        float dx = x - (float)a;
        float dy = y - (float)b;
        return (dx*gradient.x + dy*gradient.y);
    }

    static float smoothstep(float edge0, float edge1, float x) {
        x = Math.max(0, Math.min(1, (x - edge0) / (edge1 - edge0)));
        return x * x * (3 - 2 * x);
    }

    static float perlin(float x, float y, int seed) {
        int x0 = (int)x;
        int x1 = x0 + 1;
        int y0 = (int)y;
        int y1 = y0 + 1;
        float sx = x - (float)x0;
        float sy = y - (float)y0;
        sx = smoothstep(0, 1, sx);
        sy = smoothstep(0, 1, sy);
        float n0, n1, val1, val2;
        n0 = randomGrD(x0, y0, x, y, seed);
        n1 = randomGrD(x1, y0, x, y, seed);
        val1= (n0-n1)*sx+n0;
        n0 = randomGrD(x0, y1, x, y, seed);
        n1 = randomGrD(x1, y1, x, y, seed);
        val2= (n0-n1)*sx+n0;
        return (val1-val2)*sy+val1;
    }

    public static float map(float value, float currentRangeStart, float currentRangeEnd, float newRangeStart, float newRangeEnd) {
        return (value - currentRangeStart) * (newRangeEnd - newRangeStart) / (currentRangeEnd - currentRangeStart) + newRangeStart;
    }

    public static void main(String[] args) {
        int width = 1024;
        int height = 1024;

        BufferedImage image = new BufferedImage(width, height, BufferedImage.TYPE_INT_RGB);

        // Generate a random seed
        int seed = random.nextInt();

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                float total = 0;
                float max_value = 0;
                float amplitude = 1;
                for (int o = 0; o < octaves; o++) {
                    total += perlin(x * (1 << o) / (float)width, y * (1 << o) / (float)height, seed) * amplitude;
                    max_value += amplitude;
                    amplitude *= persistence;
                }
                total /= max_value;
                total = total * 0.5f + 0.5f;  // Scale the result to be in the range 0 - 1

                // Map the values to a grayscale color and set the pixel
                int colorValue = (int) map(total, 0, 1, 0, 255);

                // Ensure colorValue is within the acceptable range
                colorValue = Math.max(0, Math.min(255, colorValue));

                Color color = new Color(colorValue, colorValue, colorValue);
                image.setRGB(x, y, color.getRGB());
            }
        }

        File outputfile = new File("perlin_noise_" + seed + ".png");
        try {
            ImageIO.write(image, "png", outputfile);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
