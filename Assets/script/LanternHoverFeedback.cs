using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LanternHoverFeedback : MonoBehaviour
{
    [Header("视觉反馈设置")]
    public Color normalColor = Color.red;          // 正常颜色
    public Color hoverColor = Color.yellow;        // 悬停颜色
    public float colorChangeSpeed = 2f;           // 颜色变化速度

    [Header("声音反馈设置")]
    public AudioClip hoverEnterSound;             // 悬停开始音效
    public AudioClip hoverExitSound;              // 悬停结束音效

    [Header("特效反馈")]
    public ParticleSystem hoverParticles;         // 悬停粒子特效
    public Light hoverLight;                      // 悬停灯光效果

    // 私有变量
    private XRSimpleInteractable simpleInteractable;
    private Renderer lanternRenderer;
    private AudioSource audioSource;
    private Material lanternMaterial;
    private Color targetColor;
    private bool isHovering = false;

    void Start()
    {
        // 获取组件引用
        InitializeComponents();

        // 设置初始状态
        SetupInitialState();

        // 订阅XR事件
        SubscribeToEvents();
    }

    void Update()
    {
        // 平滑颜色过渡
        UpdateColorTransition();
    }

    /// <summary>
    /// 初始化所有需要的组件
    /// </summary>
    private void InitializeComponents()
    {
        simpleInteractable = GetComponent<XRSimpleInteractable>();
        lanternRenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();

        // 如果没有AudioSource，自动添加一个
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1.0f; // 3D音效
            audioSource.volume = 0.7f;
        }

        // 获取材质实例
        if (lanternRenderer != null)
        {
            lanternMaterial = lanternRenderer.material;
        }
    }

    /// <summary>
    /// 设置初始状态
    /// </summary>
    private void SetupInitialState()
    {
        // 设置初始颜色
        if (lanternMaterial != null)
        {
            lanternMaterial.color = normalColor;
            targetColor = normalColor;
        }

        // 禁用初始特效
        if (hoverParticles != null && hoverParticles.isPlaying)
            hoverParticles.Stop();

        if (hoverLight != null)
            hoverLight.enabled = false;
    }

    /// <summary>
    /// 订阅XR交互事件
    /// </summary>
    private void SubscribeToEvents()
    {
        if (simpleInteractable != null)
        {
            // 悬停开始事件
            simpleInteractable.hoverEntered.AddListener(OnHoverStarted);
            // 悬停结束事件
            simpleInteractable.hoverExited.AddListener(OnHoverEnded);
        }
    }

    /// <summary>
    /// 悬停开始时的处理
    /// </summary>
    private void OnHoverStarted(HoverEnterEventArgs args)
    {
        Debug.Log($"开始悬停: {gameObject.name}");

        isHovering = true;
        targetColor = hoverColor;

        // 播放悬停音效
        if (hoverEnterSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverEnterSound);
        }

        // 启动粒子特效
        if (hoverParticles != null && !hoverParticles.isPlaying)
        {
            hoverParticles.Play();
        }

        // 开启灯光
        if (hoverLight != null)
        {
            hoverLight.enabled = true;
        }
    }

    /// <summary>
    /// 悬停结束时的处理
    /// </summary>
    private void OnHoverEnded(HoverExitEventArgs args)
    {
        Debug.Log($"结束悬停: {gameObject.name}");

        isHovering = false;
        targetColor = normalColor;

        // 播放悬停结束音效
        if (hoverExitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverExitSound);
        }

        // 停止粒子特效
        if (hoverParticles != null && hoverParticles.isPlaying)
        {
            hoverParticles.Stop();
        }

        // 关闭灯光
        if (hoverLight != null)
        {
            hoverLight.enabled = false;
        }
    }

    /// <summary>
    /// 更新颜色过渡效果
    /// </summary>
    private void UpdateColorTransition()
    {
        if (lanternMaterial != null)
        {
            lanternMaterial.color = Color.Lerp(
                lanternMaterial.color,
                targetColor,
                Time.deltaTime * colorChangeSpeed
            );
        }
    }

    /// <summary>
    /// 脚本销毁时取消事件订阅
    /// </summary>
    private void OnDestroy()
    {
        if (simpleInteractable != null)
        {
            simpleInteractable.hoverEntered.RemoveListener(OnHoverStarted);
            simpleInteractable.hoverExited.RemoveListener(OnHoverEnded);
        }
    }

    /// <summary>
    /// 在Inspector中调试用的方法
    /// </summary>
    [ContextMenu("测试悬停效果")]
    public void TestHoverEffect()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("请在运行模式下测试悬停效果");
            return;
        }

        // 模拟悬停开始
        var testArgs = new HoverEnterEventArgs();
        OnHoverStarted(testArgs);

        // 2秒后模拟悬停结束
        Invoke("TestHoverEnd", 2f);
    }

    private void TestHoverEnd()
    {
        var testArgs = new HoverExitEventArgs();
        OnHoverEnded(testArgs);
    }
}